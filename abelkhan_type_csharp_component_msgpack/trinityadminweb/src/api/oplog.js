import request from '@/utils/request'

export function fetchList(query) {
  return request({
    url: '/oplog/list',
    method: 'post',
    data: query
  })
}
