import { connect } from 'react-redux';
import Shipment from '../components/Shipment';
import { enterEditMode } from '../actionCreators';

const mapDispatchToProps = dispatch => {
    return {
        onEdit: id => {
            dispatch(enterEditMode(id));
        }
    }
};

export default connect(null, mapDispatchToProps)(Shipment);