import React, { Component } from 'react';
import ReturnedCar from './ReturnedCar';

class Return extends Component {
    constructor() {
        super();
        this.state = {
            bookingNumber: "",
            newMilage: ""
        };
    }

    handleChangeBooking = (e) => {
        this.setState({ bookingNumber: e.target.value })
    }
    handleChangeMilage = (e) => {
        this.setState({ newMilage: e.target.value })
    }
    submitHandler = (e) => {
        e.preventDefault();
        if (this.state.bookingNumber && this.state.newMilage)
            this.props.returnCar(this.state.bookingNumber, this.state.newMilage);
    }
    render() {
        return (
            <div>
                <div>
                    <h3>
                        Lämna bil här:
                </h3>
                </div>
                <form onSubmit={this.submitHandler}>
                    <div>
                        <h6>
                            Vilket var ditt bokningsnummer?
                        </h6>
                        <div>
                            <input type="text" placeholder="Bokningsnummer" onChange={this.handleChangeBooking} required />
                        </div>
                        <h6>
                            Vad är bilens nuvarande km-antal
                        </h6>
                        <div>
                            <input type="text" placeholder="Nuvarande kmantal" onChange={this.handleChangeMilage} required></input>
                        </div>
                    </div>
                    <input type="submit" value="Lämna bil" />
                </form >
                <div>
                    <ReturnedCar returnedCar={this.props.returnedCar}></ReturnedCar>
                </div>
            </div>
        );
    }
}

export default Return;