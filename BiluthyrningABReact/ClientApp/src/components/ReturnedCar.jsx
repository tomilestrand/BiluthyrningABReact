import React, { Component } from 'react';

class ReturnedCar extends Component {

    render() {
        if (this.props.returnedCar.status)
            return (
                <div>
                    <div>
                        Din bil har återlämnats, det totala priset blir {this.props.returnedCar.totalPrice} kr
                    </div>
                </div>)
        else
            return (<div></div>)
    }
}

export default ReturnedCar;