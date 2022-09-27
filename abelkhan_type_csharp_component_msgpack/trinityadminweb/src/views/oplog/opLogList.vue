<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.userName" style="width: 200px;" class="filter-item" placeholder="用户名称" @keyup.enter.native="handleFilter" />
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">查找</el-button>
    </div>

    <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;margin-top:30px;" @sort-change="sortChange">
      <el-table-column align="center" label="用户名" width="120">
        <template slot-scope="scope">
          <span>{{ scope.row.userName }}</span>
        </template>
      </el-table-column>
      <el-table-column width="180px" align="center" label="操作">
        <template slot-scope="scope">
          <span>{{ scope.row.action }}</span>
        </template>
      </el-table-column>
      <el-table-column width="180px" align="center" label="ip">
        <template slot-scope="scope">
          <span>{{ scope.row.ip }}</span>
        </template>
      </el-table-column>
      <el-table-column width="180px" align="center" label="操作时间">
        <template slot-scope="scope">
          <span>{{ scope.row.operationTime | parseTime('{y}-{m}-{d} {h}:{i}:{s}') }}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" label="请求参数">
        <template slot-scope="scope">
          <span>{{ scope.row.reqParams }}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" label="返回code">
        <template slot-scope="scope">
          <span>{{ scope.row.respStr }}</span>
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination-container">
      <!-- <el-pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />-->
      <el-pagination background layout="total, sizes, prev, pager, next, jumper" :page-size="listQuery.limit" :page-sizes="[10,20,30, 50]" :current-page="listQuery.page" :total="total" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
    </div>
  </div>
</template>

<script>
import { fetchList } from '@/api/oplog'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'

export default {
  name: 'ComplexTable',
  // components: { Pagination },
  directives: { waves },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'info',
        deleted: 'danger'
      }
      return statusMap[status]
    },
    rolesFilter(status) {
      const statusMap = {
        mgr: 'success',
        editor: 'info',
        admin: 'danger'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 20,
        userName: undefined,
        sort: '+operationTime'
      },
      showReviewer: false
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      fetchList(this.listQuery).then(response => {
        this.list = response.data.list
        this.total = response.data.total
        this.listLoading = false
      })
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.getList()
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    sortChange(data) {
      const { prop, order } = data
      if (prop === 'name') {
        this.sortByID(order)
      }
    },
    sortByID(order) {
      if (order === 'ascending') {
        this.listQuery.sort = '+name'
      } else {
        this.listQuery.sort = '-name'
      }
      // this.handleFilter()
    },
    resetTemp() {
      this.temp = {
        uid: undefined,
        name: '',
        roles: '',
        password: '',
        avatar: '',
        introduction: ''
      }
    },
    resetDeleteTemp(uid, name) {
      this.deleteTemp = {
        uid: uid,
        name: name
      }
    },
    formatJson(filterVal) {
      return this.list.map(v => filterVal.map(j => {
        if (j === 'timestamp') {
          return parseTime(v[j])
        } else {
          return v[j]
        }
      }))
    },
    getSortClass: function(key) {
      const sort = this.listQuery.sort
      return sort === `+${key}` ? 'ascending' : 'descending'
    }
  }
}
</script>
