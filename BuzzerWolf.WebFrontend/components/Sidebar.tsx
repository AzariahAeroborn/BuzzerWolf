'use client';

import Link from 'next/link';
import { useAuth } from '@/context/AuthContext';

const Sidebar = () => {
  const { logout } = useAuth();

  return (
    <div className="w-64 h-screen bg-sidebar-bg text-sidebar-text flex flex-col p-4">
      <h1 className="text-xl font-bold mb-6">BuzzerWolf</h1>
      <nav className="flex-grow">
        <ul className="space-y-4">
          <li>
            <Link href="/home" className="block px-4 py-2 rounded hover:bg-sidebar-hover">
              Home
            </Link>
          </li>
          <li>
            <Link href="/current-season" className="block px-4 py-2 rounded hover:bg-sidebar-hover">
              Current Season
            </Link>
          </li>
          <li>
            <button
              onClick={logout}
              className="block px-4 py-2 rounded hover:bg-sidebar-hover text-left w-full"
            >
              Log Out
            </button>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;
