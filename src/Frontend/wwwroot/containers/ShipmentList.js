import { connect } from 'react-redux';
import ShipmentList from '../components/ShipmentList';

export default connect(state => ({ shipments: state.shipments.items }))(ShipmentList);