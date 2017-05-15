import { connect } from 'react-redux';
import ShipmentList from '../components/ShipmentList';

const mapStateToProps = (state) => {
    return {
        shipments: state.shipments
    }
};

const mapDispatchToProps = (dispatch) => {
    return {
    
    }
};

const ContainerShipmentList = connect(
    mapStateToProps,
    mapDispatchToProps
)(ShipmentList);

export default ContainerShipmentList;