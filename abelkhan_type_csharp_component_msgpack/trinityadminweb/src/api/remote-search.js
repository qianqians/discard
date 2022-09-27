import request from '@/utils/request'

export function searchUser(name) {
  return request({
    url: '/search/user',
    method: 'post',
    data: { name }
  })
}

export function transactionList(query) {
  return request({
    url: '/transaction/list',
    method: 'post',
    data: query
  })
}
