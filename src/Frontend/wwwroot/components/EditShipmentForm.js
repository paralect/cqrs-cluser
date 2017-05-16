import React from 'react';
import { Field } from 'redux-form';

export default ({ handleSubmit }) => {
    return (
        <form onSubmit={handleSubmit} className="form-inline">
            <Field name="address" component="input" type="text" className="form-control" />
            <button type="submit" className="btn btn-primary">
                Update
            </button>
        </form>
    );
};