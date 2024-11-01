import { OrderedMenuItemDto } from "./OrderedMenuItemDto";

export interface GetAllOrdersDto {
    id: string;
    totalPrice: number;
    orderDate: Date;
    validUntil: Date;
    isCancelled: boolean;
    orderedMenuItems: OrderedMenuItemDto[]
}