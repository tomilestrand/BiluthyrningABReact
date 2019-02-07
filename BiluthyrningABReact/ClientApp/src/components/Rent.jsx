import React, { Component } from 'react';
import BookedCar from './BookedCar';

export default class Rent extends Component {

    constructor(props) {
        super(props);
        this.state = {
            carType: "1",
            SSN: ""
        };
    }

    handleChangeCarType = (e) => {
        this.setState({ carType: e.target.value })
    }

    handleChangeSSN = (e) => {
        this.setState({ SSN: e.target.value })
    }

    submitHandler = (e) => {
        e.preventDefault();
        console.log(this.state);
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
                                <select value={this.state.carType} onChange={this.handleChangeCarType}>
                                    <option value="1">Liten bil</option>
                                    <option value="2">Van</option>
                                    <option value="3">Minibuss</option>
                                </select>
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
                <div>
                    <BookedCar bookedCar={this.props.bookedCar}></BookedCar>
                </div>
            </div>
        );
    }
}

