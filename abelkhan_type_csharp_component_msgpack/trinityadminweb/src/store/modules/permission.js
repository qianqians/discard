import { asyncRoutes, constantRoutes } from '@/router'
import path from 'path'

/**
 * Use meta.role to determine if the current user has permission
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * @param roles
 * @param route
 */
function hasUserPermission(userRoutes, routePath) {
  return userRoutes.some(rolePath => rolePath === routePath)
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

export function filterAsyncUserRoutes(routes, userRoutes, basePath = '/') {
  const res = []

  routes.forEach(route => {
    const routePath = path.resolve(basePath, route.path)
    const tmp = { ...route }
    if (hasUserPermission(userRoutes, routePath)) {
      if (tmp.children) {
        tmp.children = filterAsyncUserRoutes(tmp.children, userRoutes, routePath)
      }
      res.push(tmp)
    }
  })

  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}

const actions = {
  generateRoutes({ commit }, { roles, routes }) {
    return new Promise(resolve => {
      let accessedRoutes
      if (roles.includes('admin')) {
        accessedRoutes = asyncRoutes || []
      } else {
        if (routes && routes instanceof Array && routes.length > 0) {
          accessedRoutes = filterAsyncUserRoutes(asyncRoutes, routes)
        } else {
          accessedRoutes = filterAsyncRoutes(asyncRoutes, [roles])
        }
      }
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
