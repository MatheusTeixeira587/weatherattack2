import React, { Component } from 'react'
import { connect } from 'react-redux'
import { withRouter, Redirect } from 'react-router-dom'
import { bindActionCreators } from 'redux';
import { getLocationAction, showLoaderAction, hideLoaderAction, changeFieldAction, requestLoginAction, requestRegisterAction, triggerRegisterDisplayAction } from '../../../actions';
import { LoginComponent, RegisterComponent, Link, WeatherCardComponent } from '../../';
import { Grid } from '@material-ui/core';
import { routes, createAccountMessage, alreadyHaveAccountMessage, noWeatherDescriptionAvailableMessage } from '../../../constants';

import backgroundImageSvg  from '../../../static/img/background.svg';
import logoSvg from '../../../static/img/logo.svg';

const styles = {
  divStyle: {
    height: '100vh',
    boxShadow: '0 0px 80px rgba(0, 0, 0, 0.7)',
    zIndex: 1,
  },
  background: {
    background: `url('../../..${backgroundImageSvg}')`,
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center',
    overflow: 'hidden',
  },
  logo: {
    background: `url('../../..${logoSvg}')`,
    backgroundSize:'cover 100%',
    backgroundRepeat:'no-repeat',
    backgroundPosition: 'center',
    height: '100%',
  },
  logoContainer: {
    height: '200px', 
  },
}

class LoginPage extends Component {

  constructor(props) {
    super(props)
    this.renderLoginOrRegisterComponent = this.renderLoginOrRegisterComponent.bind(this);
    this.renderWeatherCard = this.renderWeatherCard.bind(this);
  }

  renderLoginOrRegisterComponent() {
    if (this.props.login.shouldRenderRegister)
      return (
        <RegisterComponent
          login={this.props.login}
          changeField={this.props.changeFieldAction}
          requestRegisterAction={this.props.requestRegisterAction}
        />
      )

    return (
      <LoginComponent
        login={this.props.login}
        changeField={this.props.changeFieldAction}
        requestLoginAction={this.props.requestLoginAction}
      />
    )
  }

  renderWeatherCard() {
    if (this.props.weather.cityName) {
      return (
        <WeatherCardComponent 
          city={this.props.weather.cityName}
          wind={parseInt(this.props.weather.wind.speed, 10)}
          description={this.props.weather.weather.length > 0 ? this.props.weather.weather[0].description : noWeatherDescriptionAvailableMessage}
          temperature={parseInt(this.props.weather.main.temperature, 10)}
          onClick={this.props.getLocationAction}
          icon={this.props.weather.weather[0].icon}
        />
      )
    }
  }
  
  render() {
    if (this.props.login.token) {
        return <Redirect to={routes.DASHBOARD_PAGE} />
    }
    return (
      <Grid
        container
        item
        direction="row"
        justify="center"
        lg={12}
        sm={12}
      >
        <Grid
          component="div"
          container
          direction="row"
          alignContent="flex-start"
          alignItems="center"
          justify="center"
          lg={4}
          sm={12}
          item
          style={styles.divStyle}
        >
          <Grid
            component='div'
            item
            lg={12}
            sm={12}
          >
          <Grid
            component='div'
            style={styles.logoContainer}
          >
            <div
              style={styles.logo}
            >
            </div>
          </Grid>
          </Grid>
          {this.renderLoginOrRegisterComponent()}
          <Link
            onClick={this.props.triggerRegisterDisplayAction}
            message={this.props.login.shouldRenderRegister ? alreadyHaveAccountMessage : createAccountMessage}
          />
        </Grid>
        <Grid
          item
          lg={8}
          container
          style={styles.background}
        >
        {this.renderWeatherCard()}
        </Grid>     
      </Grid>
    )
  }
}

const mapStateToProps = state => ({ 
  loader: state.loaderReducer,
  login: state.loginReducer,
  geolocation: state.geolocationReducer,
  weather: state.weatherReducer,
  authorization: state.authorizationReducer
});

const mapDispatchToProps = dispath =>
bindActionCreators(
  {
    showLoaderAction, 
    hideLoaderAction, 
    changeFieldAction, 
    requestLoginAction, 
    triggerRegisterDisplayAction, 
    requestRegisterAction,
    getLocationAction
  }, dispath)

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(LoginPage))