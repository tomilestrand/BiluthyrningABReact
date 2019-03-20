import React, { Component } from 'react';

export default class AddCar extends Component {

    constructor(props) {
        super(props);
        this.state = {
            carType: "",
            regNum: "",
            numOfKm: ""
        };
    }

    handleChangeCarType = (e) => {
        this.setState({ carType: e.target.value })
    }

    handleChangeregNum = (e) => {
        this.setState({ regNum: e.target.value })
    }

    handleChangenumOfKm = (e) => {
        this.setState({ numOfKm: e.target.value })
    }
    submitHandler = (e) => {
        e.preventDefault();
        if (this.state.carType)
            this.props.addCar(this.state.carType, this.state.regNum,this.state.numOfKm);
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
                                <select value={this.state.carType} onChange={this.handleChangeCarType}>
                                    <option value="1">Liten bil</option>
                                    <option value="2">Van</option>
                                    <option value="3">Minibuss</option>
                                </select>
                            </div>
                            <h6>
                                Skriv in bilens registreringsnummer
                            </h6>
                            <div>
                                <input type="text" placeholder="Registreringsnummer" onChange={this.handleChangeregNum} required />
                            </div>
                            <h6>
                                Skriv in bilens nuvarande kilometerantal
                            </h6>
                            <div>
                                <input type="text" placeholder="Kilometerantal" onChange={this.handleChangenumOfKm} required />
                            </div>
                        </div>
                        <div>
                            <input type="submit" value="Lägg till bil" />
                        </div>
                    </form >
                </div>
            </div>
        );
    }
}

