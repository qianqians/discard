<template>
  <div class="mixin-components-container">
    <el-row>
      <el-card class="box-card">
        <div slot="header" class="clearfix">
          <span>服务</span>
        </div>
        <div style="margin-bottom:50px;">
          <el-col :span="4" class="text-center">
            <el-button class="pan-btn pink-btn" @click="handleCmd('close', 'close')">
              关闭服务
            </el-button>
          </el-col>
          <el-col :span="4" class="text-center">
            <el-button class="pan-btn yellow-btn" @click="handleCmd('reload', 'reload')">
              服务reload
            </el-button>
          </el-col>
          <!--
          <el-col :span="4" class="text-center">
            <router-link class="pan-btn pink-btn" to="/excel/export-excel">
              Excel
            </router-link>
          </el-col>
          <el-col :span="4" class="text-center">
            <router-link class="pan-btn green-btn" to="/table/complex-table">
              Table
            </router-link>
          </el-col>
          <el-col :span="4" class="text-center">
            <router-link class="pan-btn tiffany-btn" to="/example/create">
              Form
            </router-link>
          </el-col>
          <el-col :span="4" class="text-center">
            <router-link class="pan-btn yellow-btn" to="/theme/index">
              Theme
            </router-link>
          </el-col>
          -->
        </div>
      </el-card>
    </el-row>
    <el-row :gutter="20" style="margin-top:50px;">
      <el-card class="box-card">
        <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;" @sort-change="sortChange">
          <el-table-column label="服务名称" prop="name" sortable="custom" align="center" :class-name="getSortClass('name')">
            <template slot-scope="{row}">
              <span>{{ row.name }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" label="服务类型">
            <template slot-scope="scope">
              <span>{{ scope.row.type }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" label="服务子类型">
            <template slot-scope="scope">
              <span>{{ scope.row.hub_type }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" label="服务ip">
            <template slot-scope="scope">
              <span>{{ scope.row.ip }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" label="服务端口">
            <template slot-scope="scope">
              <span>{{ scope.row.port }}</span>
            </template>
          </el-table-column>
          <el-table-column label="操作" align="center" width="360" class-name="small-padding fixed-width">
            <template slot-scope="{row,$index}">
              <el-button type="primary" size="mini" @click="handleServiceReload(row)">
                重加载
              </el-button>
              <el-button v-if="row.status!='deleted'" size="mini" type="danger" @click="handleServiceClose(row,$index)">
                关闭
              </el-button>
              <el-button size="mini" type="success" @click="operation(row.name, row.hub_type)">
                操作
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </el-row>
    <!--
    <el-row :gutter="20" style="margin-top:50px;">
      <el-col :span="6">edit(scope.row.uid, scope.row.userName, scope.row.role, scope.row.locked)
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>Material Design 的input</span>
          </div>
          <div style="height:100px;">
            <el-form :model="demo" :rules="demoRules">
              <el-form-item prop="title">
                <md-input v-model="demo.title" icon="el-icon-search" name="title" placeholder="输入标题">
                  标题
                </md-input>
              </el-form-item>
            </el-form>
          </div>
        </el-card>
      </el-col>

      <el-col :span="6">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>图片hover效果</span>
          </div>
          <div class="component-item">
            <pan-thumb width="100px" height="100px" image="https://wpimg.wallstcn.com/577965b9-bb9e-4e02-9f0c-095b41417191">
              vue-element-admin
            </pan-thumb>
          </div>
        </el-card>
      </el-col>

      <el-col :span="6">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>水波纹 waves v-directive</span>
          </div>
          <div class="component-item">
            <el-button v-waves type="primary">
              水波纹效果
            </el-button>
          </div>
        </el-card>
      </el-col>

      <el-col :span="6">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>hover text</span>
          </div>
          <div class="component-item">
            <mallki class-name="mallki-text" text="vue-element-admin" />
          </div>
        </el-card>
      </el-col>
    </el-row>
    -->
  </div>
</template>

<script>
// import PanThumb from '@/components/PanThumb'
// import MdInput from '@/components/MDinput'
// import Mallki from '@/components/TextHoverEffect/Mallki'
// import DropdownMenu from '@/components/Share/DropdownMenu'
import waves from '@/directive/waves/index.js' // 水波纹指令
import { doCmd, doDispatcher } from '@/api/gm'

export default {
  name: 'Trinity',
  components: {
    // PanThumb,
    // MdInput,
    // Mallki
  },
  directives: {
    waves
  },
  data() {
    const validate = (rule, value, callback) => {
      if (value.length !== 6) {
        callback(new Error('请输入六个字符'))
      } else {
        callback()
      }
    }
    return {
      cmd: {
        cmdName: '',
        param: ''
      },
      demo: {
        title: ''
      },
      demoRules: {
        title: [{ required: true, trigger: 'change', validator: validate }]
      },
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 20,
        importance: undefined,
        title: undefined,
        type: undefined,
        sort: '+name'
      }
    }
  },
  created() {
    this.doGetServiceList('GetServiceList', '')
  },
  methods: {
    operation(name, hub_type) {
      let path = ''
      let pathName = ''
      if (hub_type === 'data') {
        path = '/gm/trinityData'
        pathName = 'trinityData'
      } else if (hub_type === 'match') {
        path = '/gm/trinityMatch'
        pathName = 'trinityMatch'
      } else if (hub_type === 'http_gate') {
        path = '/gm/trinityGate'
        pathName = 'trinityGate'
      } else if (hub_type === 'scene') {
        path = '/gm/trinityScene'
        pathName = 'trinityScene'
      }
      if (path) {
        this.$router.push({
          path: path,
          name: pathName,
          params: {
            name: name,
            type: hub_type
          }
        })
      } else {
        this.$notify({
          title: 'warning',
          message: '无对应类型' + hub_type + '的hub跳转',
          type: 'warning',
          duration: 2000
        })
      }
    },
    handleCmd(cmdName, param) {
      this.$confirm('是否执行名: ' + cmdName + '?', 'Warning', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await this.doCmd(cmdName, param)
        })
        .catch(err => { console.log('取消执行' + err) })
    },
    async doCmd(cmdName, param) {
      doCmd({ cmdName: cmdName, param: param }).then(response => {
        this.$notify({
          title: 'Success',
          message: '命令执行成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    async doGetServiceList(cmdName, param) {
      this.listLoading = true
      doDispatcher({ cmdName: cmdName, param: JSON.stringify(param) }).then(response => {
        this.list = response.data
        this.total = response.data.length
        this.listLoading = false
      })
    },
    async doDispatcher(cmdName, param) {
      doDispatcher({ cmdName: cmdName, param: JSON.stringify(param) }).then(response => {
        this.$notify({
          title: 'Success',
          message: '命令执行成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    handleDispatcher(cmdName, param) {
      this.$confirm('是否执行名: ' + cmdName + '?', 'Warning', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await this.doDispatcher(cmdName, param)
        })
        .catch(err => { console.log('Dispatcher 取消执行' + err) })
    },
    handleServiceClose(row) {
      this.$confirm('是否确定需要关闭该{' + row.name + '}服务', 'Warning', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await this.doDispatcher('ServiceClose', { name: row.name, type: row.type })
          this.doGetServiceList('GetServiceList', '')
        })
        .catch(err => { console.log('Dispatcher 取消执行' + err) })
    },
    handleServiceReload(row) {
      this.$confirm('是否确定需要重加载该{' + row.name + '}服务', 'Warning', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(async() => {
          await this.doDispatcher('ServiceReload', { name: row.name, type: row.type })
          this.doGetServiceList('GetServiceList', '')
        })
        .catch(err => { console.log('Dispatcher 取消执行' + err) })
    },
    getSortClass: function(key) {
      const sort = this.listQuery.sort
      return sort === `+${key}` ? 'ascending' : 'descending'
    },
    sortChange(data) {
      const { prop, order } = data
      if (prop === 'name') {
        this.sortByID(order)
      }
    }
  }
}
</script>

<style scoped>
.mixin-components-container {
  background-color: #f0f2f5;
  padding: 30px;
  min-height: calc(100vh - 84px);
}
.component-item{
  min-height: 100px;
}
</style>
