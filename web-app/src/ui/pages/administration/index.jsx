import React from "react"
import { bindActionCreators } from "redux"
import { withRouter, Redirect } from "react-router-dom"
import { connect } from "react-redux"
import { withStyles } from "@material-ui/core/styles"
import { Grid } from "@material-ui/core"
import { BACKGROUND } from "../../../static"
import { AppBarWithMenu, AdministrationContent } from "../../components"
import { routes, permissionLevel } from "../../../constants"

const styles = {
    page: {
        minHeight: "100vh",
        display: "flex",
        flexDirection: "column",
    },
    contentWrapper: {
        background:`url("${BACKGROUND}")`,
        width: "100%",
        backgroundSize: "cover",
        backgroundRepeat: "no-repeat",
        minHeight: "100%",
        flex: 1
    }
}

class AdministratorPage extends React.Component {

    constructor(props) {
        super(props)

        this.requiredAuthorization = this.requiredAuthorization.bind(this)
    }

    requiredAuthorization() {
        console.log(this.props)
        if (!this.props.login.token) {
            return <Redirect to={routes.LOGIN_PAGE} />
        }

        if (this.props.login.permissionLevel !== permissionLevel.ADMIN) {
            return <Redirect to={routes.DASHBOARD_PAGE} />
        }
    }

    render() {
        const { classes } = this.props
        return (
            <Grid className={classes.page}>
                {this.requiredAuthorization()}
                <div className={classes.contentWrapper}>
                    <AppBarWithMenu 
                    />
                    <AdministrationContent 
                    />
                </div>
            </Grid>
        )
    }
}

const mapStateToProps = state => ({ 
    loader: state.loaderReducer,
    login: state.loginReducer,
    challenge: state.challengeReducer
})
  
const mapDispatchToProps = dispath =>
    bindActionCreators({

    },
    dispath)
  
export default withStyles(styles)(withRouter(connect(mapStateToProps, mapDispatchToProps)(AdministratorPage)))