import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router) { }

  users: any[] = [];
  isSignup = false;

  signup_obj = {
    username: '',
    email: '',
    password: ''
  };

  user_obj = {
    username: '',
    password: ''
  };

  ngOnInit(): void {
    // Load users from local storage when the component initializes so that when we refresh
    //it doesn't lose the users that were added
    const storedUsers = localStorage.getItem('users');
    if (storedUsers) {
      this.users = JSON.parse(storedUsers);
    }
  }

  toggleForm(signup: boolean) {
    this.isSignup = signup;
  }

  OnSignUp() {
    if (this.signup_obj.username && this.signup_obj.email && this.signup_obj.password) {
      this.users.push({ ...this.signup_obj }); 
      localStorage.setItem('users', JSON.stringify(this.users)); 
      alert('Sign-up successful! You can now log in.');
      this.signup_obj = { username: '', email: '', password: '' }; 
    } else {
      alert('Please fill in all fields.');
    }
  }

  OnLogin() {
    const foundUser = this.users.find(user => 
      user.username === this.user_obj.username && 
      user.password === this.user_obj.password
    );

    if (foundUser) {
      //alert('Login successful!'); 
      this.router.navigate(['/products']);
    } else {
      alert('Invalid username or password.');
    }

    this.user_obj = { username: '', password: '' }; 
  }
}
