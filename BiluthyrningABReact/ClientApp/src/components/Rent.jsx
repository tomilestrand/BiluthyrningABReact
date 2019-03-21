import React, { Component } from 'react';
import BookedCar from './BookedCar';

let newIndex = 0;

export default class Rent extends Component {

    constructor(props) {
        super(props);
        this.state = {
            carType: "1",
            SSN: "",
            regNum: ""
        };
    }

    handleChange = (e) => {
        this.setState({
            [e.target.name]: e.target.type === 'checkbox' ? e.target.checked : e.target.value
        });
    }

    submitCarTypeHandler = (e) => {
        e.preventDefault();
        this.props.getAvailableCars(this.state.carType)
    }

    submitHandler = (e) => {
        e.preventDefault();
        if (this.state.SSN !== "" && this.state.regNum !== "")
            this.props.bookCar(this.state.regNum, this.state.SSN);
    }

    render() {
        return (
            <div>
                <h3>
                    Hyr bil här:
                </h3>
                <div>
                    <form onSubmit={this.submitCarTypeHandler}>
                        <div>
                            <h6>
                                Vilken typ av bil vill du hyra?
                            </h6>
                            <div>
                                <select name="carType" value={this.state.carType} onChange={this.handleChange}>
                                    <option value="1">Liten bil</option>
                                    <option value="2">Van</option>
                                    <option value="3">Minibuss</option>
                                </select>
                            </div>
                            <div>
                                <input type="submit" value="Välj biltyp" />
                            </div>
                        </div>
                    </form >
                    <div>
                        <ul>
                            {this.props.availableCars.map((car) => {
                                if (car.regNum) {
                                    return <li onClick={() => this.setState({ regNum: car.regNum })} key={newIndex++}>{car.regNum + " " + car.numOfKm + "km"}</li>
                                }
                                else {
                                    return <li key={newIndex++}>Inga tillgängliga bilar</li>
                                }
                            }
                            )}
                        </ul>
                    </div>
                    <form onSubmit={this.submitHandler}>
                        <h6>
                            Skriv in ditt personnummer
                            </h6>
                        <div>
                            <input type="text" name="SSN" placeholder="Personnummer" onChange={this.handleChange} required />
                        </div>
                        <div>
                            <input type="submit" value="Boka bil" />
                        </div>
                    </form>
                </div>
                <div>
                    <BookedCar pickedCar={this.state.regNum} bookedCar={this.props.bookedCar}></BookedCar>
                </div>
            </div>
        );
    }
}

