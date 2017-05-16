import { connect } from 'react-redux';
import Shipment from '../components/Shipment';
import { enterEditMode } from '../actionCreators';

export default connect(null, { onEdit: enterEditMode })(Shipment);