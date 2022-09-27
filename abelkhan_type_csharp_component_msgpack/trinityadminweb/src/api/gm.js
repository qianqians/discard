import request from '@/utils/request'

export function doCmd(data) {
  return request({
    url: '/gm/cmd',
    method: 'post',
    data
  })
}

export function doDispatcher(data) {
  return request({
    url: '/gm/dispatcher',
    method: 'post',
    data
  })
}

export function doDispatcherHub(data) {
  return request({
    url: '/gm/dispatcherHub',
    method: 'post',
    data
  })
}
