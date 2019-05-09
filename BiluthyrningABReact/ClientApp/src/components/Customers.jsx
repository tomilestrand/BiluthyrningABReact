import React, { Component } from 'react';

let newIndex = 0;
let bookingIndex = 0;

export default class Customers extends Component {
    constructor(props) {
        super(props);
        this.props.getCustomers();
    }

    render() {
        if (this.props.customers.length > 0) {
            return (
                <div>
                    <h5>
                        Kundlista
                    </h5>
                    <ul>
                        {this.props.customers.map((customer) => {
                            return <li onClick={() => this.props.getCustomerBookings(customer.ssn)} className="customer" key={newIndex++}>{customer.firstName + " " + customer.lastName + " " + customer.ssn}</li>
                        }
                        )}
                    </ul>
                    < ul >
                        {this.props.customerBookings.map(function (booking) {
                            if (booking.milesDriven) {
                                return <li key={bookingIndex++}>{booking.regNum + " " + booking.carType + " " + booking.milesDriven + "km"}</li>
                            } else {
                                return <li key={bookingIndex++}>{booking.regNum + " " + booking.carType + " pågående " + booking.carbookingId}</li>
                            }
                        }
                        )}
                    </ul>
                </div>
            );
        } else {
            return (
                <div>Inga resultat</div>
            )
        }
    }
}