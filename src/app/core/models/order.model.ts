export interface OrderItem {
  productId: number;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: number;
  userId: number;
  totalAmount: number;
  orderDate: string;
  status: string;
  items: OrderItem[];
}
