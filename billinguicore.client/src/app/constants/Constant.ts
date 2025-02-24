export const Constant = {
  COMMON_API_URL: 'https://localhost:7000/',

  VIDEO_CONNECT_RATE: {
    GET_ALL_PLANS: 'api/RatePlan/searchPlan',
    UPDATE_BILIING_RATE_PLAN: 'api/RatePlan/updateBillingRate',
    ADD_BILLING_RATE_PLAN: 'api/RatePlan/addBillingRate',
    GET_ALL_PLAN_SITE: 'api/RatePlan/searchSite',
    ADD_SITE: 'api/RatePlan/addSite',
    UPDATE_SITE: 'api/RatePlan/updateSite',
    GET_EVENTS_HISTORY: 'api/Event/getEventsHistory',
    GET_SITE_By_SITE_ID: 'api/RatePlan/getSiteById',
    GET_COMM_TYPE: 'api/RatePlan/getCommType'
  },

  AUTH_USER: {
    GET_USER: 'api/Home/getUser',
    SEARCH_USER: 'api/User/searchUser',
    GET_ROLES: 'api/User/getRoles',
    ADD_USER: 'api/User/addUser',
    DELETE_USER: 'api/User/deleteUser',
  },

  TEXT_VIDEO_CONNECT: {
    API_URL: '',
    GET_ALL_TEXT_RATE: ''
  },

  PAYMENT_CONFIG: {
    API_URL: '',
    PAYMENT_CONFIG_RATE: ''
  },

  ENROLLEE_PAY_RATE: {
    API_URL: '',
    ENROLLEE_PAY_PLAN: ''
  },
  ROLE_PERMISSION: {
    ADMINISTRATOR: 'Administrator',
    READONLY:'ReadOnly'

  }
}
