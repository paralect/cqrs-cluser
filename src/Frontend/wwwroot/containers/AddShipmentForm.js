import React from 'react';
import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { addShipment } from '../actionCreators';
import AddShipmentForm from '../components/AddShipmentForm';

export default connect(state => {
    return {
        errorMessage: state.shipments.errorMessage
    }
}, {
    onSubmit: addShipment
})(reduxForm({
    form: 'addShipmentForm'
})(AddShipmentForm));