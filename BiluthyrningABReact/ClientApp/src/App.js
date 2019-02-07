import React, { Component } from 'react';
import './App.css';
import Rent from './components/Rent';
import Return from './components/Return';
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
            error: "",
            activeRents: []
        };
        this.getActiveRents();
    }

    returnCar = (carbookingId, newMilage) => {
        var data = {
            "CarbookingId": carbookingId,
            "NewMilage": newMilage,
        };
        fetch("returncar", {
            // mode: 'cors',
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

    bookCar = (carType, SSN) => {
        var data = {
            "SSN": SSN,
            "CarType": carType
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
                console.log(response);
                if (response.json) {
                    return response.json();
                }
            })
            .then((response) => {
                console.log(response)
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
                    <Link to="/bookcar">Hyr bil</Link>
                    <Link to="/returncar">Lämna tillbaka</Link>
                    <Link to="/customerlist">Kunder</Link>
                    <section>
                        <Route
                            path="/bookcar"
                            render={(props) => <Rent {...props} bookCar={this.bookCar} bookedCar={this.state.bookedCar} />}
                        />
                        <Route
                            path="/returncar"
                            render={(props) => <Return {...props} returnCar={this.returnCar} returnedCar={this.state.returnedCar} />}
                        />
                        <Route
                            path="/customerlist"
                            render={(props) => <Customers {...props} getCustomers={this.getCustomers} customers={this.state.customers} getCustomerBookings={this.getCustomerBookings} customerBookings={this.state.customerBookings} />}
                        />
                    </section>
                {this.state.activeRents.regNum}
                </div>
            </Router>
        );
    }
}