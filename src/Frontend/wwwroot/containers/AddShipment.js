import React from 'react';
import { connect } from 'react-redux';
import { addShipment } from '../actions';

let AddShipment = ({ dispatch }) => {
    let input;

    return (
        <div>
            <form onSubmit={e => {
                e.preventDefault();
                if (!input.value.trim()) {
                    return;
                }
                dispatch(addShipment(input.value));
                input.value = '';
            }}>

                <h3>Add new shipment:</h3>

                <div className="form-group row">
                    <label>Shipment address</label>
                    <input ref={node => {input = node}} className="form-control" />
                </div>

                <button type="submit" className="btn btn-primary">
                    Add Shipment
                </button>

            </form>
        </div>
    );
};

AddShipment = connect()(AddShipment);

export default AddShipment;