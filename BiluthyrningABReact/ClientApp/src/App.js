import React, { Component } from 'react';
import React, { Component } from 'react';
import './App.css';
import Rent from '.components/Rent';
import Return from '.components/Return';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
//import { Layout } from './components/Layout';
//import { Home } from './components/Home';
//import { FetchData } from './components/FetchData';
//import { Counter } from './components/Counter';

export default class App extends Component {
    constructor(props) {
        super(props);
        this.bookCar = this.bookCar.bind(this);
        this.returnCar = this.returnCar.bind(this);
        this.state = {
            bookedCar: {},
            returnedCar: {},
            error: ""
        };
    }

    returnCar(carbookingId, newMilage) {
        var data = {
            "CarbookingId": carbookingId,
            "NewMilage": newMilage,
        };
        fetch("https://localhost:44370/returncar", {
            // mode: 'cors',
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
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

    bookCar(carType, SSN) {
        var data = {
            "SSN": SSN,
            "CarType": carType,
        };
        fetch("https://localhost:44370/rentcar", {
            // mode: 'cors',
            method: 'Post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
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

    render() {
        return (
            <Router>
                <div className="App">
                    <Link to="/rentcar">Hyr bil</Link>
                    <Link to="/returncar">Lämna tillbaka</Link>
                    <section>
                        <Route
                            path="/rentcar"
                            render={(props) => <Rent {...props} bookCar={this.bookCar} bookedCar={this.state.bookedCar} />}
                        />
                        <Route
                            path="/returncar"
                            render={(props) => <Return {...props} returnCar={this.returnCar} returnedCar={this.state.returnedCar} />}
                        />
                    </section>
                </div>
            </Router>
        );
    }
}