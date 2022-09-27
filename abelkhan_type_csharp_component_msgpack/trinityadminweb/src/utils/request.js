import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'

// create an axios instance
const service = axios.create({
  // baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 5000 // request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    config.baseURL = store.getters.host || process.env.VUE_APP_BASE_API
    config.headers['Content-Type'] = 'application/json;charset=utf-8'
    if (store.getters.token) {
      config.headers['X-Token'] = getToken()
    }
    return config
  },
  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

service.interceptors.response.use(
  response => {
    const res = response.data
    if (res.code === 0) {
      return { code: res.code, data: res.data }
    } else {
      if (response.status === 400) {
        return Promise.reject()
      }
      if (res.code === 2 || response.status === 403 || response.status === 402) {
        return MessageBox.confirm('尚未登陆或超时，可以取消继续留在该页面，或者重新登录', '确定登出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('user/fedLogout').then(() => {
            location.reload() // 为了重新实例化vue-router对象 避免bug
          })
        })
      } else {
        if (res.code === 3) {
          return MessageBox.confirm('登录信息已过期，请重新登录', '确定登出', {
            confirmButtonText: '重新登录',
            type: 'warning'
          }).then(() => {
            store.dispatch('user/fedLogout').then(() => {
              location.reload() // 为了重新实例化vue-router对象 避免bug
            })
          })
        } else if (res.code === -3) {
          Message({
            message: res.errMessage,
            type: 'error',
            duration: 5 * 1000
          })
        } else if (res.code === -1) {
          Message({
            message: res.errMessage,
            type: 'error',
            duration: 5 * 1000
          })
        } else if (res.errMessage) {
          Message({
            message: res.errMessage,
            type: 'error',
            duration: 5 * 1000
          })
        }
      }
      return Promise.reject('error')
    }
  },
  (error) => {
    if (error.response && error.response.data) {
      if (error.response.data.statusCode === 401 &&
        error.response.data.error === 'Unauthorized') {
        return MessageBox.confirm('你已被登出，可以取消继续留在该页面，或者重新登录', '确定登出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('user/fedLogout').then(() => {
            location.reload() // 为了重新实例化vue-router对象 避免bug
          })
        })
      } else {
        Message({
          message: error.response.data.message,
          type: 'error',
          duration: 5 * 1000
        })
      }
    } else {
      Message({
        message: error.message,
        type: 'error',
        duration: 5 * 1000
      })
    }
    return Promise.reject(error)
  })

export default service
