<template>
  <user-detail :is-edit="true" />
</template>

<script>
import userDetail from './components/userDetail'

export default {
  name: 'UserInfo',
  components: { userDetail }
}
</script>

