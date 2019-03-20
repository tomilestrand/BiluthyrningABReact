import React, { Component } from 'react';

class BookedCar extends Component {

    carType(car) {
        switch (car) {
            case 1:
                return "liten bil";
            case 2:
                return "van";
            case 3:
                return "minibus";
            default:
                return "okänd biltyp"
        }
    }

    render() {
        if (this.props.bookedCar.status === "OK")
            return (
                <div>
                    <div>
                        Din bokade {this.carType(this.props.bookedCar.carType)} har registreringsnummer: {this.props.bookedCar.regNum}
                    </div>
                    <div>
                        Ditt bokningsnummer är {this.props.bookedCar.carbookingId}
                    </div>
                </div>)
        else if (this.props.pickedCar !== "") {
            return (<div>
                Du har valt bilen med registreringsnummer: {this.props.pickedCar}
            </div>)
        } else {
            return (<div></div>)
        }
    }
}

export default BookedCar;