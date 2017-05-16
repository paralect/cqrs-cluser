import React from 'react';
import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { updateShipment } from '../actionCreators';
import EditShipmentForm from '../components/EditShipmentForm';

const mapStateToProps = (state, ownProps) => ({
    initialValues: {
        address: ownProps.address
    }
});

export default connect(mapStateToProps, {
    onSubmit: updateShipment
})(reduxForm({})(EditShipmentForm));