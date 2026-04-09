import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/product.service';
import { CartService } from '../../../core/cart.service';
import { Product } from '../../../core/models/product.model';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  loading = false;
  error = '';
  success = '';

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAllProducts().subscribe({
      next: (res) => {
        this.products = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load products';
        this.loading = false;
      },
    });
  }

  addToCart(productId: number): void {
    const payload = {
      productId: productId,
      quantity: 1,
    };

    this.cartService.addToCart(payload).subscribe({
      next: () => {
        this.success = 'Product added to cart';
        setTimeout(() => (this.success = ''), 2000);
      },
      error: () => {
        this.error = 'Failed to add product to cart';
        setTimeout(() => (this.error = ''), 2000);
      },
    });
  }
}
