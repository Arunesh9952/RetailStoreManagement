import { Component } from '@angular/core';
import { OrderService } from '../../../core/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent {
  shippingAddress = '';
  paymentMethod = 'Cash on Delivery';

  constructor(private orderService: OrderService, private router: Router) {}

  placeOrder(): void {
    const payload = {
      shippingAddress: this.shippingAddress,
      paymentMethod: this.paymentMethod,
    };

    this.orderService.placeOrder(payload).subscribe(() => {
      alert('Order placed successfully');
      this.router.navigate(['/orders']);
    });
  }
}
