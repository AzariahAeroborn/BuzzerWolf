import React, { Component } from 'react';
import axios from 'axios';

export class Login extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userName: '',
            accessKey: '',
            loggedIn: false
        };
    }

    login = (e) => {
        axios({
            method: 'post',
            url: '/api/login',
            data: {
                userName: this.state.userName,
                accessKey: this.state.accessKey
            }
        })
        .then(() => this.setState({ loggedIn: true }))
        .then(() => axios({
            method: 'get',
            url: '/api/team'
        }))

        e.preventDefault();
    }

    render() {
        return (
            <form onSubmit={this.login}>
                <label htmlFor="username">User Name: </label><input type="text" id="username" value={this.state.userName} onChange={(event) => this.setState({ userName: event.target.value })} /><br />
                <label htmlFor="accesskey">Access Key: </label><input type="text" id="accesskey" value={this.state.accessKey} onChange={(event) => this.setState({ accessKey: event.target.value })} /><br />
                <input type="submit" value="Login" />
            </form>
        );
    }
}