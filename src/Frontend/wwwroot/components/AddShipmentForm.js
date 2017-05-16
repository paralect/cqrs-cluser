import React from 'react';
import { Field } from 'redux-form';

export default ({ handleSubmit, errorMessage }) => {
    return (
        <form onSubmit={handleSubmit}>
            <h3>Add new shipment:</h3>
            <div className={"form-group row " + (errorMessage ? 'has-danger' : '')} >
                <label htmlFor="shipmentAddress">Shipment address</label>
                <Field name="shipmentAddress" component="input" type="text" className="form-control" />
                { errorMessage && <div className="form-control-feedback">{errorMessage}</div> }
            </div>
            <button type="submit" className="btn btn-primary">
                Add Shipment
            </button>
        </form>
    );
};