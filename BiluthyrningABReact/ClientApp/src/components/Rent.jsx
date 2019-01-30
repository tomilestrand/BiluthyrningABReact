import React, { Component } from 'react';
import BookedCar from './BookedCar';

class Rent extends Component {

    constructor(props) {
        super(props);
        this.submitHandler = this.submitHandler.bind(this);
        this.handleChangeCarType = this.handleChangeCarType.bind(this);
        this.handleChangeSSN = this.handleChangeSSN.bind(this);
        this.state = {
            carType: "",
            SSN: ""
        };
    }

    handleChangeCarType(e) {
        this.setState({ carType: e.target.value })
    }

    handleChangeSSN(e) {
        this.setState({ SSN: e.target.value })
    }

    submitHandler(e) {
        e.preventDefault();
        if (this.state.carType)
            this.props.bookCar(this.state.carType, this.state.SSN);
    }

    render() {
        return (
            <div>
                <h3>
                    Hyr bil här:
                </h3>
                <div>
                    <form onSubmit={this.submitHandler}>
                        <div>
                            <h6>
                                Vilken typ av bil vill du hyra?
                            </h6>
                            <div>
                                <input type="text" placeholder="Biltyp" onChange={this.handleChangeCarType} required />
                            </div>
                            <h6>
                                Skriv in ditt personnummer
                            </h6>
                            <div>
                                <input type="text" placeholder="Personnummer" onChange={this.handleChangeSSN} required />
                            </div>
                        </div>
                        <div>
                            <input type="submit" value="Välj bil" />
                        </div>
                    </form >
                </div>
                <p>
                    Välj 1 för liten bil, välj 2 för van, välj 3 för minibuss
                </p>
                <div>
                    <BookedCar bookedCar={this.props.bookedCar}></BookedCar>
                </div>
            </div>
        );
    }
}

export default Rent;