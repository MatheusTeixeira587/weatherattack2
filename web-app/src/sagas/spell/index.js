import { takeLatest, select, put, take, all } from "redux-saga/effects"
import { types } from "../../constants"
import { assignPagedSpells } from "../../actions"
import { SpellService } from "../../services"

const spellService = new SpellService()

function* addSpellSaga(action){
    debugger
    const token = yield select(state => state.loginReducer.token)
    try {
        yield spellService.put(action, token)
    } catch (e) {
        console.log(e)
    }
}

function* getPagedSpellSaga(action) {
    debugger
    const token = yield select(state => state.loginReducer.token)
    try {
        const response = yield spellService.pagedGet(token, action.command.pageNumber)
        yield put(assignPagedSpells(response))

    } catch (e) {
        console.log(e)
    }
}

export function* watchSpellSaga() {
    yield takeLatest(types.ADD_SPELL, addSpellSaga)
    yield take(types.GET_PAGED_SPELLS, getPagedSpellSaga)
}