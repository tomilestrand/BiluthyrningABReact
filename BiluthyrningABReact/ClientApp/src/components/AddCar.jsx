import React, { Component } from 'react';

export default class AddCar extends Component {

    constructor(props) {
        super(props);
        this.state = {
            carType: "1",
            regNum: "",
            numOfKm: "0",
            retire: false
        };
    }

    carResponse = (r) => {
        if (!r) {
            return (<div></div>)
        } else {
            return (<div>{this.props.addedCar.status}</div>)
        }
    }

    handleChange = (e) => {
        this.setState({
            [e.target.name]: e.target.type === 'checkbox' ? e.target.checked : e.target.value
        });
    }

    submitHandler = (e) => {
        e.preventDefault();
        if (!this.state.retire) {
            this.props.addCar(this.state.carType, this.state.regNum, this.state.numOfKm);
        }
        else {
            this.props.retireCar(this.state.carType, this.state.regNum);
        }
    }

    render() {
        return (
            <div>
                <h3>
                    Lägg till bil här:
                </h3>
                <div>
                    <form onSubmit={this.submitHandler}>
                        <div>
                            <h6>
                                Vilken typ av bil vill du lägga till?
                            </h6>
                            <div>
                                <select name="carType" value={this.state.carType} onChange={this.handleChange}>
                                    <option value="1">Liten bil</option>
                                    <option value="2">Van</option>
                                    <option value="3">Minibuss</option>
                                </select>
                            </div>
                            <h6>
                                Skriv in bilens registreringsnummer
                            </h6>
                            <div>
                                <input name="regNum" type="text" placeholder="Registreringsnummer" onChange={this.handleChange} required />
                            </div>
                            <h6>
                                Skriv in bilens nuvarande kilometerantal
                            </h6>
                            <div>
                                <input name="numOfKm" type="text" placeholder="Kilometerantal" onChange={this.handleChange} required />
                            </div>
                        </div>
                        <div>
                            <h6>
                                Skall en redan existerande bil tas bort ur systemet?
                            </h6>
                            <input
                                name="retire"
                                type="checkbox"
                                checked={this.state.retire}
                                onChange={this.handleChange} />
                        </div>
                        <div>
                            <input type="submit" value="Lägg till bil" />
                        </div>
                    </form >
                    <div>
                        {this.carResponse(this.props.addedCar)}
                    </div>
                </div>
            </div>
        );
    }
}

