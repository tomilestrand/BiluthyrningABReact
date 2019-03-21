import React, { Component } from 'react';
import './App.css';
import Rent from './components/Rent';
import Return from './components/Return';
import AddCar from './components/AddCar';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import Customers from './components/Customers';

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            bookedCar: {},
            returnedCar: {},
            customers: [],
            customerBookings: [],
            addedCar: "",
            error: "",
            activeRents: [],
            availableCars:[]
        };
        this.getActiveRents();
    }

    returnCar = (carbookingId, newMilage) => {
        var data = {
            "CarbookingId": carbookingId,
            "NewMilage": newMilage,
        };
        fetch("returncar", {
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': "application/json; charset=utf-8"
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((response) => {
                if (response.status === "OK") {
                    this.setState({ returnedCar: response });
                } else {
                    this.setState({ returnedCar: response.status })
                }
            })
    };

    bookCar = (regNum, SSN) => {
        var data = {
            "SSN": SSN,
            "RegNum": regNum
        };
        fetch("rentcar", {
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': "application/json; charset=utf-8"
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((response) => {
                if (response.status === "OK") {
                    this.setState({ bookedCar: response });
                } else {
                    this.setState({ bookedCar: response.status })
                }
            })
    };

    getAvailableCars = (carType) => {
        var data = {
            "CarType": carType
        };
        fetch("availablecars", {
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': "application/json; charset=utf-8"
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((response) => {
                if (response.status === "OK") {
                    this.setState({ availableCars: response.cars });
                } else {
                    this.setState({ availableCars: [response.status] })
                }
            })
    };

    addCar = (carType, regNum, numOfKm) => {
        var data = {
            "RegNum": regNum,
            "NumOfKm":numOfKm,
            "CarType": carType
        };
        fetch("addcar", {
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': "application/json; charset=utf-8"
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                return response.json();
            })
            .then((response) => {
                if (response.status === "OK") {
                    this.setState({ addedCar: response });
                } else {
                    this.setState({ addedCar: response.status })
                }
            })
    };

    getActiveRents = ()  => {
        fetch("activerents", {
            method: 'Get',
            headers: {
                'Accept': 'application/json',
            }
        })
            .then((response) => {
                return response.json();
            })
            .then((response => {
                this.setState({ activeRents: response.activeRents });
            }))
    };

    getCustomers = () => {
        fetch("customers", {
            method: 'Get',
            headers: {
                'Accept': 'application/json',
            }
        })
            .then((response) => {
                return response.json();
            })
            .then((response => {
                this.setState({ customers: response.customers });
            }))
    };

    getCustomerBookings = (SSN) => {
        var data = {
            "SSN": SSN,
        };
        fetch("customers", {
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': "application/json; charset=utf-8"
            },
            body: JSON.stringify(data)
        })
            .then((response) => {
                if (response.json) {
                    return response.json();
                }
            })
            .then((response) => {
                if (response.status === "OK") {
                    this.setState({ customerBookings: response.customerBookings });
                } else {
                    this.setState({ customerBookings: response.status })
                }
            })
    };

    render() {
        return (
            <Router>
                <div className="App">
                    <Link className="App-link" to="/">Hyr bil</Link>
                    <Link className="App-link" to="/returncar">Lämna tillbaka</Link>
                    <Link className="App-link" to="/customerlist">Kunder</Link>
                    <Link className="App-link" to="/addcar">Hantera bilar</Link>
                    <section>
                        <Route
                            exact path="/"
                            render={(props) => <Rent {...props} getAvailableCars={this.getAvailableCars} availableCars={this.state.availableCars} bookCar={this.bookCar} bookedCar={this.state.bookedCar} />}
                        />
                        <Route
                            path="/returncar"
                            render={(props) => <Return {...props} returnCar={this.returnCar} returnedCar={this.state.returnedCar} />}
                        />
                        <Route
                            path="/customerlist"
                            render={(props) => <Customers {...props} getCustomers={this.getCustomers} customers={this.state.customers} getCustomerBookings={this.getCustomerBookings} customerBookings={this.state.customerBookings} />}
                        />
                        <Route
                            path="/addcar"
                            render={(props) => <AddCar {...props} addCar={this.addCar} addedCar={this.state.addedCar} />}
                        />
                    </section>
                {this.state.activeRents.regNum}
                </div>
            </Router>
        );
    }
}