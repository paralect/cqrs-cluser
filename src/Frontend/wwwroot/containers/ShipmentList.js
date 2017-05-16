import { connect } from 'react-redux';
import ShipmentList from '../components/ShipmentList';

const mapStateToProps = state => {
    return {
        shipments: state.shipments.items
    }
};

export default connect(mapStateToProps)(ShipmentList);