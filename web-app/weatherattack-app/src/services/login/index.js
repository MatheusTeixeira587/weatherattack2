import { BaseService } from "../_base";

export class LoginService extends BaseService {
    
    constructor() {
        super("api/Authentication");
    }

    login(user) {
        //todo
        console.log(user);
    }
}