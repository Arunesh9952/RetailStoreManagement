import { Component, OnInit } from '@angular/core';
import { CartService } from '../../../core/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-page',
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css'],
})
export class CartPageComponent implements OnInit {
  cart: any;
  total = 0;

  constructor(private cartService: CartService, private router: Router) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe((res) => {
      this.cart = res;
      this.calculateTotal();
    });
  }

  calculateTotal(): void {
    if (!this.cart?.items) {
      this.total = 0;
      return;
    }

    this.total = this.cart.items.reduce((sum: number, item: any) => {
      return sum + item.product.price * item.quantity;
    }, 0);
  }

  removeItem(productId: number): void {
    this.cartService.removeFromCart(productId).subscribe(() => {
      this.loadCart();
    });
  }

  goToCheckout(): void {
    this.router.navigate(['/checkout']);
  }
}
