import { types } from "../../constants"

const initialState = {
    pageCount: 0,
    pageSize: 20,
    pageNumber: 1,
    users: [],
    totalRecords: 0
}

export function userAreaReducer(state = initialState, action) {

    switch(action.type) {
        case types.ASSIGN_PAGED_USERS:
            return Object.assign({}, {
                users: action.command.result,
                pageNumber: action.command.pageNumber,
                pageCount: action.command.pageCount,
                pageSize: action.command.pageSize,
                totalRecords: action.command.totalRecords
            })

        case types.CHANGE_ROWS_PAGE:
            return Object.assign({}, state, {
                pageSize: action.command.result
            })
            
        default:
            return state
    }
}