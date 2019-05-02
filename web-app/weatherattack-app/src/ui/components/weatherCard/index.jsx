import React from 'react';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import { Card, CardContent, CardMedia, Typography, Button } from '@material-ui/core';
import { weatherButtonText } from '../../../constants'
import sun from '../../../static/icons/sun.svg';

const styles = theme => ({
  card: {
    display: 'flex',
    top: '10px',
    right: '10px',
    position: 'absolute',
    overflow: 'hidden',
    borderRadius: '20px',
    background: 'linear-gradient(rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0.3))',
  },
  details: {
    display: 'flex',
    flexDirection: 'column',
    opacity: 1,
    backgroundAttachment: 'fixed'
  },
  content: {
    flex: '1 0 auto',
    opacity: 1,
    color: 'white'
  },
  '@keyframes spin-0': {
    from: {
      transform: 'rotate(0deg)',
      opacity: 1,
    },
    to: {
      transform: 'rotate(360deg)',opacity: 0,
    }
  },
  cover: {
    width: 121,
    opacity: 1,
    backgroundSize: 'contain 100%',
    overflow: 'hidden',
    animationName: '$spin-0',
  },
});

const WeatherCard = props => {
  const { classes, temperature, description, wind, city, onClick } = props;
  let icon = props.icon;

  switch (icon) {
    
    default:
      icon = sun;
  }

  return (
    <Card className={classes.card}>
      <div className={classes.details}>
        <CardContent className={classes.content}>
          <Typography component="h5" variant="h5" color="inherit">
            {temperature+"°C"}
          </Typography>
          <Typography variant="body1" color="inherit">
            {description}
          </Typography>
          <Typography variant="subtitle2" color="inherit">
            {wind+"km/h"}
          </Typography>
          <Typography variant="body2" color="inherit">
            {city}
          </Typography>
          <Button
            variant="contained"
            onClick={onClick}
          >
            <span>{weatherButtonText}</span>
          </Button>
        </CardContent>
      </div>
      <CardMedia
        className={classes.cover}
        image={icon}
        title="Weather Icon"
      />
    </Card>
  );
}

WeatherCard.propTypes = {
  classes: PropTypes.object.isRequired,
  theme: PropTypes.object.isRequired,
  temperature: PropTypes.number.isRequired,
  description: PropTypes.string.isRequired,
  wind: PropTypes.number.isRequired,
  city: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired
};
export default withStyles(styles, { withTheme: true })(WeatherCard);