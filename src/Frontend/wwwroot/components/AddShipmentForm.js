import React from 'react';
import { Field } from 'redux-form';

const AddShipmentForm = props => {
    return (
        <form onSubmit={props.handleSubmit}>
            <h3>Add new shipment:</h3>
            <div className="form-group row">
                <label htmlFor="shipmentAddress">Shipment address</label>
                <Field name="shipmentAddress" component="input" type="text" className="form-control" />
            </div>
            <button type="submit" className="btn btn-primary">
                Add Shipment
            </button>
        </form>
    );
};

export default AddShipmentForm;