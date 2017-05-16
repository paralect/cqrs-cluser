import React from 'react';
import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { addShipment } from '../actions';
import AddShipmentForm from '../components/AddShipmentForm';

export default connect(null, {
    onSubmit: addShipment
})(reduxForm({
    form: 'addShipmentForm'
})(AddShipmentForm));