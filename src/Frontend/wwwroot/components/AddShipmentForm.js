import React from 'react';
import { Field } from 'redux-form';

export default props => {
    return (
        <form onSubmit={props.handleSubmit}>
            <h3>Add new shipment:</h3>
            <div className={"form-group row " + (props.errorMessage ? 'has-danger' : '')} >
                <label htmlFor="shipmentAddress">Shipment address</label>
                <Field name="shipmentAddress" component="input" type="text" className="form-control" />
                { props.errorMessage && <div className="form-control-feedback">{props.errorMessage}</div> }
            </div>
            <button type="submit" className="btn btn-primary">
                Add Shipment
            </button>
        </form>
    );
};