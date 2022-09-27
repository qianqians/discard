import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/user/login',
    method: 'post',
    data: data
  })
}

export function getInfo(token) {
  return request({
    url: '/user/info',
    method: 'post',
    data: { token }
  })
}

export function logout() {
  return request({
    url: '/user/logout',
    method: 'post'
  })
}

export function createUser(data) {
  return request({
    url: '/user/create',
    method: 'post',
    data: data
  })
}
export function fetchList(query) {
  return request({
    url: '/user/list',
    method: 'post',
    data: query
  })
}

export function updateUser(data) {
  return request({
    url: '/user/update',
    method: 'post',
    data: data
  })
}

export function deleteUser(data) {
  return request({
    url: '/user/delete',
    method: 'post',
    data
  })
}

export function lockedUser(data) {
  return request({
    url: '/user/lock',
    method: 'post',
    params: data
  })
}

export function getUserInfo() {
  return request({
    url: '/user/info',
    method: 'post'
  })
}

export function updateSelfUser(data) {
  return request({
    url: '/user/updateSelf',
    method: 'post',
    data
  })
}

export function updateUserPwd(data) {
  return request({
    url: '/user/updatePwd',
    method: 'post',
    data
  })
}
