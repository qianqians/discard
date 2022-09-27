import request from '@/utils/request'

export function reqPermissions() {
  return request({
    url: '/role/permissions',
    method: 'post'
  })
}

export function getRoutes() {
  return request({
    url: '/role/routes',
    method: 'post'
  })
}

export function getRoles() {
  return request({
    url: '/role/list',
    method: 'post'
  })
}

export function addRole(data) {
  return request({
    url: '/role/add',
    method: 'post',
    data
  })
}

export function updateRole(data) {
  return request({
    url: '/role/update',
    method: 'post',
    data
  })
}

export function deleteRole(key) {
  return request({
    url: '/role/delete',
    method: 'post',
    data: { key }
  })
}
