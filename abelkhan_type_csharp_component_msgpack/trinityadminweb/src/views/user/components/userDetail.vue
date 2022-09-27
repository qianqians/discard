<template>
  <div class="app-container">
    <div class="filter-container">
      <sticky class-name="'sub-navbar '+postForm.status">
        <el-button v-loading="loading" style="margin-left: 10px;" type="warning" icon="el-icon-edit" @click="fetchData(true)">刷新
        </el-button>
        <el-button v-loading="loading" style="margin-left: 10px;" type="success" icon="el-icon-edit" @click="submitForm">保存
        </el-button>
        <el-button v-loading="loading" style="margin-left: 10px;" type="warning" icon="el-icon-edit" @click="handleUpdatePwd">修改密码
        </el-button>
      </sticky>
    </div>

    <el-form ref="postForm" class="form-container" :model="postForm" :rules="rules">

      <el-row>
        <el-col :span="24">
          <div class="postInfo-container">
            <el-row>
              <el-col :span="12">
                <el-form-item label-width="80px" label="用户名" class="postInfo-container-item">
                  <el-input v-model="postForm.name" readonly />
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <el-form-item label-width="80px" label="创建时间:" class="postInfo-container-item">
                  <el-input v-model="postForm.createTimeStr" readonly />
                </el-form-item>
              </el-col>
            </el-row>
          </div>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="24">
          <div class="postInfo-container">
            <el-row>
              <el-col :span="12">
                <el-form-item label-width="80px" label="登录时间:" class="postInfo-container-item">
                  <el-input v-model="postForm.loginTimeStr" readonly />
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <el-form-item label-width="80px" label="权限:" class="postInfo-container-item">
                  <el-input v-model="postForm.roles" readonly />
                </el-form-item>
              </el-col>
            </el-row>
          </div>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="24">
          <div class="postInfo-container">
            <el-row>
              <el-col :span="12">
                <el-form-item label-width="80px" label="描述:" class="postInfo-container-item">
                  <el-input v-model="postForm.introduction" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label-width="80px" label="头像:" class="postInfo-container-item">
                  <el-input v-model="postForm.avatar" />
                </el-form-item>
              </el-col>
            </el-row>
          </div>
        </el-col>
      </el-row>
    </el-form>

    <el-dialog title="修改密码" :visible.sync="dialogPwdVisible">
      <el-form :model="dialogPwdInfo" label-position="left" label-width="70px" style="width: 400px; margin-left:50px;">
        <el-form-item label="旧密码" prop="oldPassword">
          <el-input v-model="dialogPwdInfo.oldPassword" :type="passwordType" />
        </el-form-item>
        <el-form-item label="新密码" prop="password">
          <el-input v-model="dialogPwdInfo.password" :type="passwordType" />
        </el-form-item>
        <el-form-item label="确认密码" prop="confirmPassword">
          <el-input v-model="dialogPwdInfo.confirmPassword" :type="passwordType" />
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogPwdVisible = false">取消</el-button>
        <el-button type="danger" @click="updatePwd">确认</el-button>
      </span>
    </el-dialog>

  </div>
</template>

<script>
import Sticky from '@/components/Sticky' // 粘性header组件
import { validateURL } from '@/utils/validate'
import { getUserInfo, updateSelfUser, updateUserPwd } from '@/api/user'
import { parseTime } from '../../../utils'

const defaultForm = {
  uid: '',
  name: '',
  roles: '',
  avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
  introduction: '',
  loginTime: '',
  createTime: '',
  loginTimeStr: '',
  createTimeStr: ''
}

export default {
  name: 'UserDetail',
  components: { Sticky },
  props: {
    isEdit: {
      type: Boolean,
      default: false
    }
  },
  data() {
    const validateRequire = (rule, value, callback) => {
      if (value === '') {
        this.$message({
          message: rule.field + '为必传项',
          type: 'error'
        })
        callback(null)
      } else {
        callback()
      }
    }
    const validateSourceUri = (rule, value, callback) => {
      if (value) {
        if (validateURL(value)) {
          callback()
        } else {
          this.$message({
            message: '外链url填写不正确',
            type: 'error'
          })
          callback(null)
        }
      } else {
        callback()
      }
    }
    return {
      postForm: Object.assign({}, defaultForm),
      loading: false,
      userListOptions: [],
      rules: {
        image_uri: [{ validator: validateRequire }],
        title: [{ validator: validateRequire }],
        content: [{ validator: validateRequire }],
        source_uri: [{ validator: validateSourceUri, trigger: 'blur' }]
      },
      dialogPwdVisible: false,
      dialogPwdInfo: {
        oldPassword: '',
        password: '',
        confirmPassword: ''
      },
      passwordType: 'password'
    }
  },
  computed: {
    contentShortLength() {
      return this.postForm.content_short.length
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    showPwd() {
      if (this.passwordType === 'password') {
        this.passwordType = ''
      } else {
        this.passwordType = 'password'
      }
    },
    fetchData(isNotify) {
      getUserInfo().then(response => {
        this.postForm = Object.assign(this.postForm, response.data)
        this.postForm.createTimeStr = parseTime(this.postForm.createTime, '{y}-{m}-{d} {h}:{i}:{s}')
        this.postForm.loginTimeStr = parseTime(this.postForm.loginTime, '{y}-{m}-{d} {h}:{i}:{s}')
        if (isNotify) {
          this.$notify({
            title: '成功',
            message: '刷新成功',
            type: 'success',
            duration: 2000
          })
        }
      }).catch(err => {
        this.$notify({
          title: '失败',
          message: '刷新失败' + err,
          type: 'error',
          duration: 2000
        })
        console.log(err)
      })
    },
    submitForm() {
      const tempData = Object.assign({}, {
        introduction: this.postForm.introduction,
        name: this.postForm.name
      })
      this.$refs.postForm.validate(valid => {
        if (valid) {
          updateSelfUser(tempData).then(response => {
            this.loading = true
            this.$notify({
              title: '保存',
              message: '保存成功',
              type: 'success',
              duration: 2000
            })
            this.loading = false
          }).catch(err => {
            this.$notify({
              title: '保存',
              message: '保存失败' + err,
              type: 'error',
              duration: 2000
            })
            console.log(err)
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },

    handleUpdatePwd(row) {
      this.dialogPwdInfo = {
        oldPassword: '',
        password: '',
        confirmPassword: ''
      }
      this.dialogPwdVisible = true
    },
    updatePwd() {
      const tempData = {
        oldPassword: this.dialogPwdInfo.oldPassword,
        password: this.dialogPwdInfo.password,
        confirmPassword: this.dialogPwdInfo.confirmPassword
      }
      updateUserPwd(tempData).then(() => {
        this.dialogPwdVisible = false
        this.$store.dispatch('LogOut').then(() => {
          location.reload()// In order to re-instantiate the vue-router object to avoid bugs
        })
        this.$notify({
          title: '成功',
          message: '修改成功',
          type: 'success',
          duration: 2000
        })
      })
    }
  }
}
</script>

<!--
<style rel="stylesheet/scss" lang="scss" scoped>
@import "src/styles/mixin.scss";
.createPost-container {
  position: relative;
  .createPost-main-container {
    padding: 40px 45px 20px 50px;
    .postInfo-container {
      position: relative;
      @include clearfix;
      margin-bottom: 10px;
      .postInfo-container-item {
        float: left;
      }
    }
    .recommond-container {
      min-height: 500px;
      margin: 0 0 30px;
      .editor-upload-btn-container {
        text-align: right;
        margin-right: 10px;
        .editor-upload-btn {
          display: inline-block;
        }
      }
    }
  }
  .word-counter {
    width: 40px;
    position: absolute;
    right: -10px;
    top: 0px;
  }
}
</style>
-->
