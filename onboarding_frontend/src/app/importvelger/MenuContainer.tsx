interface MenuContainerProps {
    children: React.ReactNode;
}

const MenuContainer: React.FC<MenuContainerProps> = ({ children }) => {
    return (
        <div className="flex items-center justify-center min-h-[90vh]">
            <div className=" flex items-center p-6 bg-white rounded-lg shadow-md w-96">{children}</div>
        </div>
    )
};

export default MenuContainer;
