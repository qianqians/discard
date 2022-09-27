<template>
  <div class="mixin-components-container">
    <el-row>
      <el-card class="box-card">
        <div slot="header" class="clearfix">
          <span>服务</span>
        </div>
        <div style="margin-bottom:50px;">
          <el-col :span="4" class="text-center">
            <el-button class="pan-btn pink-btn" @click="handleDispatcher('SceneOk', '')">
              ok测试
            </el-button>
          </el-col>
          <el-col :span="4" class="text-center">
            <el-button class="pan-btn yellow-btn" @click="handleDispatcher('SceneOk', '')">
              服务reload
            </el-button>
          </el-col>
        </div>
      </el-card>
    </el-row>
  </div>
</template>

<script>
import waves from '@/directive/waves/index.js' // 水波纹指令
import { doDispatcherHub } from '@/api/gm'

export default {
  name: 'TrintyMatch',
  components: {
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
        param: '',
        svrName: this.$route.params.name || 'scene0',
        svrType: this.$route.params.type || 'scene'
      },
      demo: {
        title: ''
      },
      demoRules: {
        title: [{ required: true, trigger: 'change', validator: validate }]
      },
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
  },
  methods: {
    async doDispatcher(cmdName, param) {
      const temp = Object.assign({}, { cmdName: cmdName, param: JSON.stringify(param), svrName: this.cmd.svrName, svrType: this.cmd.svrType })
      doDispatcherHub(temp).then(response => {
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
