<template>
  <div class="app-container">
    <el-button type="primary" @click="handleAddRole">添加权限</el-button>

    <el-table :data="rolesList" style="width: 100%;margin-top:30px;" border>
      <el-table-column align="center" label="权限Key" width="220">
        <template slot-scope="scope">
          {{ scope.row.key }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="权限名称" width="220">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
      <el-table-column align="header-center" label="描述">
        <template slot-scope="scope">
          {{ scope.row.description }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="操作">
        <template slot-scope="scope">
          <el-button type="primary" size="small" @click="handleEdit(scope)">修改</el-button>
          <el-button type="danger" size="small" @click="handleDelete(scope)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog :visible.sync="dialogVisible" :title="dialogType==='edit'?'Edit Role':'New Role'">
      <el-form :model="role" label-width="80px" label-position="left">
        <el-form-item label="权限Key">
          <el-input v-model="role.key" placeholder="请输入key" />
        </el-form-item>
        <el-form-item label="权限名称">
          <el-input v-model="role.name" placeholder="请输入名称" />
        </el-form-item>
        <el-form-item label="权限名称">
          <el-input
            v-model="role.description"
            :autosize="{ minRows: 2, maxRows: 4}"
            type="textarea"
            placeholder="描述"
          />
        </el-form-item>
        <el-form-item label="路由权限">
          <el-tree
            ref="tree"
            :check-strictly="checkStrictly"
            :data="routesData"
            :props="defaultProps"
            show-checkbox
            node-key="path"
            class="permission-tree"
          />
        </el-form-item>
        <el-form-item label="接口权限">
          <el-tree
            ref="pTree"
            :check-strictly="checkStrictly"
            :data="permissionsData"
            :props="defaultPermissionProps"
            show-checkbox
            node-key="name"
            class="permission-tree"
          />
        </el-form-item>
      </el-form>
      <div style="text-align:right;">
        <el-button type="danger" @click="dialogVisible=false">取消</el-button>
        <el-button type="primary" @click="confirmRole">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import path from 'path'
import { deepClone } from '@/utils'
import { reqPermissions, getRoles, addRole, deleteRole, updateRole } from '@/api/role'
const { asyncRoutes } = require('@/router')

const defaultRole = {
  key: '',
  name: '',
  description: '',
  routes: [],
  permissions: []
}

export default {
  data() {
    return {
      role: Object.assign({}, defaultRole),
      routes: [],
      rolesList: [],
      permissions: [],
      dialogVisible: false,
      dialogType: 'new',
      checkStrictly: false,
      defaultProps: {
        children: 'children',
        label: 'title'
      },
      defaultPermissionProps: {
        children: '',
        label: 'des'
      }
    }
  },
  computed: {
    routesData() {
      return this.routes
    },
    permissionsData() {
      return this.permissions
    }
  },
  created() {
    // Mock: get all routes and roles list from server
    this.getRoutes()
    this.getPermissions()
    this.getRoles()
  },
  methods: {
    async getRoutes() {
      // const res = await getRoutes()
      this.serviceRoutes = asyncRoutes
      this.routes = this.generateRoutes(asyncRoutes)
    },
    async getPermissions() {
      const res = await reqPermissions()
      this.servicePermissions = res.data.list
      this.permissions = res.data.list
    },
    async getRoles() {
      const res = await getRoles()
      this.rolesList = res.data.list
    },

    // Reshape the routes structure so that it looks the same as the sidebar
    generateRoutes(routes, basePath = '/') {
      const res = []

      for (let route of routes) {
        // skip some route
        if (route.hidden) { continue }

        const onlyOneShowingChild = this.onlyOneShowingChild(route.children, route)

        if (route.children && onlyOneShowingChild && !route.alwaysShow) {
          route = onlyOneShowingChild
        }

        const data = {
          path: path.resolve(basePath, route.path),
          title: route.meta && route.meta.title

        }

        // recursive child routes
        if (route.children) {
          data.children = this.generateRoutes(route.children, data.path)
        }
        res.push(data)
      }
      return res
    },
    generateRolesRoutes(routes, roleRoutes, basePath = '/') {
      const res = []

      for (let route of routes) {
        // skip some route
        if (route.hidden) { continue }

        const onlyOneShowingChild = this.onlyOneShowingChild(route.children, route)

        if (route.children && onlyOneShowingChild && !route.alwaysShow) {
          route = onlyOneShowingChild
        }

        const data = {
          path: path.resolve(basePath, route.path),
          title: route.meta && route.meta.title
        }

        // recursive child routes
        if (route.children) {
          data.children = this.generateRolesRoutes(route.children, roleRoutes, data.path)
        }
        if (roleRoutes.some(path => data.path === path) || (data.children && data.children.length > 0)) {
          res.push(data)
        }
      }
      return res
    },
    generatePermission(permissions) {
      const res = []
      for (const permission of permissions) {
        const data = {
          name: permission,
          des: permission
        }
        res.push(data)
      }
      return res
    },
    generateArr(routes) {
      let data = []
      routes.forEach(route => {
        data.push(route)
        if (route.children) {
          const temp = this.generateArr(route.children)
          if (temp.length > 0) {
            data = [...data, ...temp]
          }
        }
      })
      return data
    },
    handleAddRole() {
      this.role = Object.assign({}, defaultRole)
      if (this.$refs.tree) {
        this.$refs.tree.setCheckedNodes([])
      }
      if (this.$refs.pTree) {
        this.$refs.pTree.setCheckedNodes([])
      }
      this.dialogType = 'new'
      this.dialogVisible = true
    },
    handleEdit(scope) {
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.checkStrictly = true
      this.role = deepClone(scope.row)
      this.$nextTick(() => {
        const routes = this.generateRolesRoutes(this.serviceRoutes, this.role.routes)
        this.$refs.tree.setCheckedNodes(this.generateArr(routes))
        const permissions = this.generatePermission(this.role.permissions)
        this.$refs.pTree.setCheckedNodes(permissions)
        this.checkStrictly = false
      })
    },
    handleDelete({ $index, row }) {
      this.$confirm('是否需要删除角色?', 'Warning', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await deleteRole(row.key)
          this.rolesList.splice($index, 1)
          this.$message({
            type: 'success',
            message: '删除成功!'
          })
        })
        .catch(err => { console.log('取消删除' + err) })
    },
    generateTree(routes, basePath = '/', checkedKeys) {
      const res = []

      for (const route of routes) {
        const routePath = path.resolve(basePath, route.path)

        let childrenArray
        // recursive child routes
        if (route.children) {
          childrenArray = this.generateTree(route.children, routePath, checkedKeys)
          res.push(...childrenArray)
        }

        if (checkedKeys.includes(routePath) || (route.children && childrenArray.length >= 1)) {
          res.push(routePath)
        }
      }
      return res
    },
    generatePerMissionTree(permissions, checkedKeys) {
      const res = []
      for (const perssion of permissions) {
        if (checkedKeys.includes(perssion.name)) {
          res.push(perssion.name)
        }
      }
      return res
    },
    async confirmRole() {
      const isEdit = this.dialogType === 'edit'

      const checkedKeys = this.$refs.tree.getCheckedKeys()
      this.role.routes = this.generateTree(deepClone(this.serviceRoutes), '/', checkedKeys)
      const checkedPKeys = this.$refs.pTree.getCheckedKeys()
      this.role.permissions = this.generatePerMissionTree(this.servicePermissions, checkedPKeys)
      if (isEdit) {
        await updateRole(this.role)
        for (let index = 0; index < this.rolesList.length; index++) {
          if (this.rolesList[index].key === this.role.key) {
            this.rolesList.splice(index, 1, Object.assign({}, this.role))
            break
          }
        }
      } else {
        await addRole(this.role)
        this.rolesList.push(this.role)
      }

      const { description, key, name } = this.role
      this.dialogVisible = false
      this.$notify({
        title: 'Success',
        dangerouslyUseHTMLString: true,
        message: `
            <div>Role Key: ${key}</div>
            <div>Role Name: ${name}</div>
            <div>Description: ${description}</div>
          `,
        type: 'success'
      })
    },
    // reference: src/view/layout/components/Sidebar/SidebarItem.vue
    onlyOneShowingChild(children = [], parent) {
      let onlyOneChild = null
      const showingChildren = children.filter(item => !item.hidden)

      // When there is only one child route, the child route is displayed by default
      if (showingChildren.length === 1) {
        onlyOneChild = showingChildren[0]
        onlyOneChild.path = path.resolve(parent.path, onlyOneChild.path)
        return onlyOneChild
      }

      // Show parent if there are no child route to display
      if (showingChildren.length === 0) {
        onlyOneChild = { ... parent, path: '', noShowingChildren: true }
        return onlyOneChild
      }

      return false
    }
  }
}
</script>

<style lang="scss" scoped>
.app-container {
  .roles-table {
    margin-top: 30px;
  }
  .permission-tree {
    margin-bottom: 30px;
  }
}
</style>
