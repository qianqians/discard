import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path(.*)',
        component: () => import('@/views/redirect/index')
      }
    ]
  },
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/auth-redirect',
    component: () => import('@/views/login/auth-redirect'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/error-page/404'),
    hidden: true
  },
  {
    path: '/401',
    component: () => import('@/views/error-page/401'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [
      {
        path: 'dashboard',
        component: () => import('@/views/dashboard/index'),
        name: 'Dashboard',
        meta: { title: '首页', icon: 'dashboard', affix: true }
      }
    ]
  },
  {
    path: '/profile',
    component: Layout,
    redirect: '/profile/index',
    hidden: true,
    children: [
      {
        path: 'index',
        component: () => import('@/views/profile/index'),
        name: 'Profile',
        meta: { title: 'Profile', icon: 'user', noCache: true }
      }
    ]
  }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [
  {
    path: '/permission',
    component: Layout,
    redirect: '/permission/userList',
    alwaysShow: true, // will always show the root menu
    name: 'Permission',
    meta: {
      title: '权限管理',
      icon: 'lock',
      roles: ['admin', 'mgr']
    },
    children: [
      {
        path: 'userList',
        component: () => import('@/views/user/userList'),
        name: 'userList',
        meta: {
          title: '用户列表',
          roles: ['admin', 'mgr']
        }
      },
      {
        path: 'role',
        component: () => import('@/views/permission/role'),
        name: 'RolePermission',
        meta: {
          title: '权限管理',
          roles: ['admin', 'mgr']
        }
      }
    ]
  },
  {
    path: '/oplog',
    component: Layout,
    redirect: '/oplog/opLogList',
    alwaysShow: true,
    name: 'OpLog',
    meta: {
      title: '操作日志',
      icon: 'chart',
      roles: ['admin', 'mgr']
    },
    children: [
      {
        path: 'opLogList',
        component: () => import('@/views/oplog/opLogList'),
        name: 'opLogList',
        meta: {
          title: '操作日志',
          roles: ['admin', 'mgr']
        }
      }
    ]
  },
  {
    path: '/gm',
    component: Layout,
    redirect: '/gm/trinity',
    alwaysShow: true,
    name: 'Gm',
    meta: {
      title: 'GM管理',
      icon: 'drag',
      roles: ['admin']
    },
    children: [
      {
        path: 'trinity',
        component: () => import('@/views/gm/trinity'),
        name: 'Trinity',
        meta: {
          title: 'GM操作',
          roles: ['admin']
        }
      },
      {
        path: 'trinityData',
        component: () => import('@/views/gm/trinityData'),
        name: 'trinityData',
        hidden: true,
        meta: {
          title: 'data服操作',
          roles: ['admin']
        }
      },
      {
        path: 'trinityGate',
        component: () => import('@/views/gm/trinityGate'),
        name: 'trinityGate',
        hidden: true,
        meta: {
          title: 'gate服操作',
          roles: ['admin']
        }
      },
      {
        path: 'trinityMatch',
        component: () => import('@/views/gm/trinityMatch'),
        name: 'trinityMatch',
        hidden: true,
        meta: {
          title: 'match服操作',
          roles: ['admin']
        }
      },
      {
        path: 'trinityScene',
        component: () => import('@/views/gm/trinityScene'),
        name: 'trinityScene',
        hidden: true,
        meta: {
          title: 'scene服操作',
          roles: ['admin']
        }
      }
    ]
  },
  {
    path: '/userSelf',
    component: Layout,
    redirect: '/userSelf/userInfo',
    alwaysShow: true, // will always show the root menu
    name: 'userInfoSelf',
    meta: {
      title: '关于我',
      icon: 'user',
      roles: ['admin', 'editor', 'mgr', 'optor', 'visitor']
    },
    children: [
      {
        path: 'userInfo',
        component: () => import('@/views/user/userInfo'),
        name: 'userInfo',
        meta: {
          title: '个人信息',
          roles: ['admin', 'editor', 'mgr', 'optor', 'visitor']
        }
      }
    ]
  },
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
